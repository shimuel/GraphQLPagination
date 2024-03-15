
Mutation - AuthorInput
----------------------
mutation pubsMutations($author: AuthorInput!){
  authorMutate{
    addAuthor(author:$author){
      id
      authorId
      name
      books{
        id
        name
      }
    }
  }
}

mutation pubsMutations($author: AuthorInput!){
  authorMutate{
    updateAuthor(author:$author){
            id
            name
            books{
              id
              name
            }
        }
    }
}

variables
---------
{
    "author": {
      	"id": 10157,
        "name": "tem101",
        "books": [
          {
            "id": "101555",
            "name": "haaamdfgfd4"
          },
          {
             "id": "10157",
            "name": "sdfsd57"
          }
        ]
    }
}

Mutation - BookInput
---------------------
mutation pubsMutations($book: BookInput!){
  bookMutate{
     addBook(book:$book){
            id
            name
            bookAuthor{
              id
              name
            }
        }
    }
}

variables
---------
{
    "book": {
        "id": "3",
        "name": "tem",
        "authorId": 4
    }
}

Query Authors - id
------------------
{
  authorQuery{
    getAuthor (id:2) {
      id
      authorId
      name
      books{
        id
        bookId
        name
      }
    }
  }
}

Query Books - id
----------
{
  bookQuery{
    bookById (id:"ISBN4") {
      id
      name
      bookAuthor{
        id
        authorId
        name
      }
    }
  }
}

Subscription
------------
subscription{
  subscriptionMessage{
    id
    messageType
    data
    at
  }
}

authors Connections - 
----------------------
{
  authorQuery{
    searchAuthor(last:3, before:/*pageInfo.cursor*/) {
       totalCount
        pageInfo{
          hasNextPage
          hasPreviousPage
          startCursor
          endCursor
        }
        edges{
          node{
            id
            authorId
            name
            books{
              id
              name
            }
          }
        }
      }
    }
}

books Connections - 
----------------------
{
  bookQuery{
    booksSearch{
       totalCount
       pageInfo{
        hasNextPage
        hasPreviousPage
        startCursor
        endCursor
      }
      edges{
        node{
          id
          bookId
          name
          bookAuthor{
            authorId
            name
          }
        }
      }
    }
  }
}

public static string GetClaimValue(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            return claimsPrincipal.Claims.Single(c => c.Type == claimType).Value;
        }

