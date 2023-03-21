
Mutation
-----
mutation PubsMutation($author: AuthorInput!){
    addAuthor(author:$author){
        id
        name
    }
}


variables
--------
{
  "author" : {
  	"id":3,
  	"name":"tem"
	}
}


Query Authors - id
----------
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

public static string GetClaimValue(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            return claimsPrincipal.Claims.Single(c => c.Type == claimType).Value;
        }