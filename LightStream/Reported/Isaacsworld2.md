# Web scraper 

With the following code, I am able to load the HTTP resources in a page using the following code: 

```csharp

    if(!Uri.IsWellFormedUriString(url, UriKind.Absolute))
    {
        Console.WriteLine($"This following is not an url: {url}");
    	return -1;
    }

    Console.WriteLine($"Preparing to scrape {url}");
    var client = new HttpClient();
    var page = client.GetStringAsync(url);
    page.Wait();
    Console.WriteLine(page.Result);
```