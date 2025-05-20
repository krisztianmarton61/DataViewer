using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using System.Text.RegularExpressions;

namespace PlaywrightTests;

public class HomeTests : PageTest
{
    [Fact]
    public async Task Home_DisplaysCorrectTitle()
    {
        await Page.GotoAsync("http://host.docker.internal:5246/");

        var title = await Page.TitleAsync();
        Assert.Equal("DataViewer | Home", title);
    }

    [Fact]
    public async Task Home_DisplaysNavigationIcons()
    {
        await Page.GotoAsync("http://host.docker.internal:5246/");

        var iconContainers = Page.Locator(".icon-container");
        await Expect(iconContainers).ToHaveCountAsync(3);

        var albums = iconContainers.Filter(new() { HasTextString = "Albums" });
        var posts = iconContainers.Filter(new() { HasTextString = "Posts" });

        await Expect(albums).ToBeVisibleAsync();
        await Expect(posts).ToBeVisibleAsync();
    }

    [Fact]
    public async Task Home_NavigateToPosts_RedirectsToPostsPage()
    {
        await Page.GotoAsync("http://host.docker.internal:5246/");

        var postsIcon = Page.Locator(".icon-container:has(h4:text('Posts'))");
        await postsIcon.WaitForAsync(new() { State = WaitForSelectorState.Visible });

        await postsIcon.ClickAsync();

        await Expect(Page).ToHaveURLAsync(new Regex(".*/posts$"), new() { Timeout = 5000 });

        Assert.EndsWith("/posts", Page.Url);
    }

    [Fact]
    public async Task Home_NavigateToAlbums_RedirectsToAlbumsPage()
    {
        await Page.GotoAsync("http://host.docker.internal:5246/");

        var albumsIcon = Page.Locator(".icon-container:has(h4:text('Albums'))");
        await albumsIcon.WaitForAsync(new() { State = WaitForSelectorState.Visible });

        await albumsIcon.ClickAsync();

        await Expect(Page).ToHaveURLAsync(new Regex(".*/albums$"), new() { Timeout = 5000 });

        Assert.EndsWith("/albums", Page.Url);
    }
}
