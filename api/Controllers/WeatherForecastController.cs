using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

using Microsoft.Extensions.ML;
//using apic.DataModels;


namespace apic.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
    public class PeopleController : ControllerBase
    {
        [HttpGet("people/all")]
        public ActionResult<IEnumerable<Person>> GetAll()
        {
            return new[]
            {
            new Person { Name = "Ana" },
            new Person { Name = "Felipe" },
            new Person { Name = "Emillia" }
        };
        }
    }

    public class Person
    {
        public string Name { get; set; }
    }

    //get
    public class BookController : ControllerBase
    {
        [HttpGet("book/all")]
        public ActionResult<IEnumerable<Book>> GetAll()
        {
            string connectionString = null;
            SqlConnection cnn;
            connectionString = "Server=localhost,1433;Database=tempdb;User Id=sa;Password=P(4)ssword;";
            cnn = new SqlConnection(connectionString);

            try
            {

                //query
                string queryString = "SELECT * FROM books;";
                SqlCommand command = new SqlCommand(queryString, cnn);
                cnn.Open();

                List<Book> myList = new List<Book>();

                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {


                        Console.WriteLine(String.Format("{0}, {1}",
                            reader[0], reader[1], reader[2]));

                        myList.Add(new Book { ID = reader.GetValue(0).ToString(), Title = reader.GetValue(1).ToString(), BookName = reader.GetValue(2).ToString() });


                    }
                }


                cnn.Close();
                return myList;

            }
            catch (Exception ex)
            {

                return new[]
                {
                    new Book { ID = "1" , Title = "failed", BookName="None"}
                };
            }


        }

        public class Book
        {
            public string ID { get; set; }
            public string Title { get; set; }
            public string BookName { get; set; }
        }

    }
    //end of get

    //post
    public class CustomersController : ControllerBase
    {
        [HttpPost("book/add")]
        public string Post([FromBody] BookInsert bookinsert)
        {
            string connectionString = null;
            SqlConnection cnn;
            connectionString = "Server=localhost,1433;Database=tempdb;User Id=sa;Password=P(4)ssword;";
            cnn = new SqlConnection(connectionString);
            try
            {
                string query = String.Format("INSERT INTO books (Title, Book) VALUES('{0}', '{1}');", bookinsert.title, bookinsert.author);
                SqlCommand command = new SqlCommand(query, cnn);
                cnn.Open();
                command.ExecuteNonQuery();
                cnn.Close();
                return "success";
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message.ToString(), "Error Message");
                return "failure";
            }
            
                
            
        }
    


        public class BookInsert
        {
            public string title { get; set; }
            public string author { get; set; }
        }
    }
    //end of post


    //post ML model
    public class PredictController : ControllerBase
    {
        private readonly PredictionEnginePool<SentimentData, SentimentPrediction> _predictionEnginePool;

        public PredictController(PredictionEnginePool<SentimentData, SentimentPrediction> predictionEnginePool)
        {
            _predictionEnginePool = predictionEnginePool;
        }

        [HttpPost("predict")]
        public ActionResult<string> Post([FromBody] SentimentData input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            
            SentimentPrediction prediction = _predictionEnginePool.Predict(modelName: "SentimentAnalysisModel", example: input);

            string sentiment = Convert.ToBoolean(prediction.Prediction) ? "Positive" : "Negative";

            return Ok(sentiment);
        }
    }
    //end of ML post


    //end
}
