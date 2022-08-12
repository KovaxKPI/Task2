# A books database web api
- Connected Entity Framework Core and used in-memory database
- Implemented the following API:
  1. Get all books. Order by provided value (title or author)
     - GET https://{{baseUrl}}/api/books?order=author
  2. Get top 10 books with high rating and number of reviews greater than 10. You can filter books by specifying genre. Order by rating
     - GET https://{{baseUrl}}/api/recommended?genre=horror
  3. Get book details with the list of reviews
     - GET https://{{baseUrl}}/api/books/{id}
  4. Delete a book using a secret key. Save the secret key in the config of your application. Compare this key with query param
     - DELETE https://{{baseUrl}}/api/books/{id}?secret=qwerty
  5. Save a new book.
     - POST https://{{baseUrl}}/api/books/save
  6. Save a review for the book.
     - PUT https://{{baseUrl}}/api/books/{id}/review
  7. Rate a book
     - PUT https://{{baseUrl}}/api/books/{id}/rate
