CREATE TABLE books (
    ID int IDENTITY(1,1) PRIMARY KEY,
    Title varchar(255),
    Book varchar(255) 
);

INSERT INTO books (Title, Book )
VALUES('Sorcer Stone', 'Harry Potter');

SELECT * FROM books;
