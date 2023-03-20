
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


Query - id
----------
{
  author(id:1){
    id
    name
  }
}

all authors
----------
{
  authors{
    id
    name
  }
}


public static string GetClaimValue(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            return claimsPrincipal.Claims.Single(c => c.Type == claimType).Value;
        }