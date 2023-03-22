
Mutation - AuthorInput
----------------------
mutation PubsMutation($author: AuthorInput!){
    addAuthor(author:$author){
        id
        name
    		books{
          id
          name
        }
    }
}

variables
---------
{
    "author": {
        "id": 3,
        "name": "tem",
        "books": [
          {
            "id": "5",
            "name": "haaamdfgfd"
          }
        ]
    }
}

Mutation - BookInput
---------------------
mutation PubsMutation($book: BookInput!){
    addBook(book:$book){
        id
        name
    		author{
          id
          name
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
  authorById(id:1){
    id
    name
    books{
      id
      name
      author{
        id
        name
      }
    }
  }
}

all authors
----------
{
  authors{
    id
    name
    books{
      id
      name
      author{
        id
        name
      }
    }
  }
}

Query Books - id
----------
{
  bookById(id:"1"){
    id
    name
    author{
      id
      name
      books {
        author{
          name
        }
      }
    }
  }
}

all books
----------
{
  books{
    id
    name
    author{
      id
      name
      books{
        id
        name
      }
    }
  }
}


Subscription
------------
subscription {
  newMessages {
    id
    message
    from
    sent
  }
}

public static string GetClaimValue(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            return claimsPrincipal.Claims.Single(c => c.Type == claimType).Value;
        }

