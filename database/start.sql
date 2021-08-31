CREATE TABLE main (
    ID int IDENTITY(1,1) PRIMARY KEY,
    Title varchar(255),
    Book varchar(255) 
);

INSERT INTO main (ID, Title, Book )
VALUES(1, 'Sorcer Stone', 'Harry Potter');

SELECT * FROM main;
