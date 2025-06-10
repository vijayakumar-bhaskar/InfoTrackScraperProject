namespace InfoTrackProject;

public interface IScraper
{
    public  Task<string> Scrape(string query);
}