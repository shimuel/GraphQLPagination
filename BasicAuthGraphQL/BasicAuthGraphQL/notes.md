
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
        "id": 4,
        "name": "tem4",
        "books": [
          {
            "id": "55",
            "name": "haaamdfgfd4"
          },
          {
             "id": "57",
            "name": "sdfsd57"
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
subscription{
	 subscriptionMessage{
    id
    MessageType
    data
    at
  }
}

public static string GetClaimValue(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            return claimsPrincipal.Claims.Single(c => c.Type == claimType).Value;
        }

