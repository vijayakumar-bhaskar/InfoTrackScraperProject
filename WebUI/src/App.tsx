import React from "react";
import "./App.css";

const App = () => {
  const [keywords, setKeywords] = React.useState("");
  const [url, setUrl] = React.useState("");

  const [isLoading, setIsLoading] = React.useState(false);
  const [hasSearched, setHasSearched] = React.useState(false);

  const [searchResults, setSearchResults] = React.useState([]);
  const [error, setError] = React.useState("");

  const handleSearch = async () => {
    // Clear previous error
    setError("");
    setHasSearched(false);

    // Validate inputs
    if (!keywords.trim() || !url.trim()) {
      setError("Both Keywords and URL fields are required");
      return;
    }

    setIsLoading(true);

    try {
      // Hard code the url for now; should be part of the .env file
      var response = await fetch("https://localhost:7242/scraper/search?query=" + encodeURI(keywords) + "&targetUrl=" + encodeURI(url) + "&pageNumber=1&pageSize=100");
      if(!response.ok) {
        throw new Error("Error " + response.status + ": " + response.statusText);
      }
      var results = await response.json();
      setSearchResults(results);
      setHasSearched(true);
    } catch (err) {
      setError((err as Error).message || "An unexpected error occurred");
      setSearchResults([]);
      setHasSearched(false);
    } finally {
      setIsLoading(false);
    }
  };

  const handleClear = () => {
    setKeywords("");
    setUrl("");
    setSearchResults([]);
    setError("");
    setHasSearched(false);
  };

  return (
      <div className="App">
        <input
            type="text"
            placeholder="Keywords"
            value={keywords}
            onChange={(e) => setKeywords(e.target.value)}
        />
        <input
            type="text"
            placeholder="URL"
            value={url}
            onChange={(e) => setUrl(e.target.value)}
        />

        {error && <div className="error-message">{error}</div>}

        <div className="button-group">
          <button
              className="clear-btn"
              onClick={handleClear}
              disabled={isLoading}
          >
            Clear
          </button>
          <button
              className="search-btn"
              onClick={handleSearch}
              disabled={isLoading}
          >
            {isLoading ? (
                <>
                  <div className="spinner"></div>
                  Searching...
                </>
            ) : (
                "Search"
            )}
          </button>
        </div>

        {hasSearched && (
            <div className="results-container">
              {searchResults.length > 0 ? (
                  <>
                    <h3>Search Results:</h3>
                    <div className="results-list">
                      {searchResults.map((number, index) => (
                          <div key={index} className="result-item">
                            {number}
                          </div>
                      ))}
                    </div>
                  </>
              ) : (
                  <div className="no-results">
                    <h3>No results found</h3>
                    <p>Try different keywords or check your URL.</p>
                  </div>
              )}
            </div>
        )}
      </div>
  );
};

export default App;
