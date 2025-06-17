# Hacker News Top Stories API

A RESTful API built with ASP.NET Core that retrieves the top N stories from the public Hacker News API, sorted by score, and returns the first N results requested by the client.

## How to Run

1. Clone the repository:
   ```bash
   git clone https://github.com/AzaelBarbosa/HackerNewsTopStories.git
   cd hackernews-api
   ```

2. Restore packages and run the project:
   ```bash
   dotnet restore
   dotnet run
   ```

3. Access the API via browser or Postman:
   ```
   GET http://localhost:5000/api/stories/{n}
   ```

   Where `{n}` is the number of stories you want to retrieve (maximum 100).

---

## Response Format

```json
[
  {
    "title": "A uBlock Origin update was rejected from the Chrome Web Store",
    "uri": "https://github.com/uBlockOrigin/uBlock-issues/issues/745",
    "postedBy": "ismaildonmez",
    "time": "2019-10-12T13:43:01+00:00",
    "score": 1716,
    "commentCount": 572
  },
  ...
]
```

---

## Assumptions

- Max 100 stories are fetched per request to avoid overloading Hacker News API.
- Best story IDs are cached in memory for 5 minutes.
- Requests are executed concurrently for performance.

---


## References

- [Hacker News API](https://github.com/HackerNews/API)
