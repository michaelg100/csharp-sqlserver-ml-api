CREATE TABLE main (
    ID int IDENTITY(1,1) PRIMARY KEY,
    Title varchar(255),
    Book varchar(255) 
);

INSERT INTO main (Title, Book )
VALUES('Sorcer Stone', 'Harry Potter');

SELECT * FROM main;
