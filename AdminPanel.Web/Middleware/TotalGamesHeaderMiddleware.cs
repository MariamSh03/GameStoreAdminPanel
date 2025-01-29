using AdminPanel.Bll.Interfaces;

namespace AdminPanel.Web.Middleware;

public class TotalGamesHeaderMiddleware
{
    private readonly RequestDelegate _next;

    public TotalGamesHeaderMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IGameService gameService)
    {
        // Calculate the total number of games
        var totalGames = await gameService.GetTotalGamesCountAsync();

        // Add the custom header to the response
        context.Response.Headers.Append("x-total-numbers-of-games", totalGames.ToString());

        // Call the next middleware in the pipeline
        await _next(context);
    }
}