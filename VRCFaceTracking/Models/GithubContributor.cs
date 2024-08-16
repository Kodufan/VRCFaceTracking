namespace VRCFaceTracking.Models;

public class GithubContributor
{
    public string login { get; set; }
    public string html_url { get; set; }
    public int contributions { get; set; }
    public string contribution_string => $" Contribution{(contributions == 1 ? "" : "s")}";
}