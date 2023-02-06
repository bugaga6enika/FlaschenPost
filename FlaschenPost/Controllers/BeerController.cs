using System;
using System.Globalization;
using FlaschenPost.Drinks;
using FlaschenPost.Drinks.Beer;
using Microsoft.AspNetCore.Mvc;

namespace FlaschenPost.Controllers
{
    [ApiController]
    [Route("api/")]
    [Produces("application/json")]
    public class BeerController : ControllerBase
    {
        private readonly BeerAggregate _beerAggregate;

        public BeerController(BeerAggregate beerAggregate)
        {
            _beerAggregate = beerAggregate ?? throw new ArgumentNullException(nameof(beerAggregate));
        }

        [HttpGet("mostExpensiveCheapest")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> GetMostExpensiveAndCheapestPerLiterSync([FromQuery] string dataSource, CancellationToken cancellationToken = default)
        {
            if (!Uri.TryCreate(dataSource, UriKind.Absolute, out var uri))
            {
                return BadRequest();
            }

            var (mostExpensive, cheapest) = await _beerAggregate.GetMostExpensiveAndCheapestAsync(uri, cancellationToken);

            if (mostExpensive is null && cheapest is null)
            {
                return NoContent();
            }

            return Ok(new { expensive = mostExpensive, cheapest });
        }

        [HttpGet("mostBottles")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> GetContainsMostBottlesAsync([FromQuery] string dataSource, CancellationToken cancellationToken = default)
        {
            if (!Uri.TryCreate(dataSource, UriKind.Absolute, out var uri))
            {
                return BadRequest();
            }

            var mostBottles = await _beerAggregate.GetContainsMostBottlesAsync(uri, cancellationToken);

            if (mostBottles is null)
            {
                return NoContent();
            }

            return Ok(mostBottles);
        }

        [HttpGet("byExactPrice")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> GetByExactPriceAsync([FromQuery] string dataSource, [FromQuery] string price = "17,99", CancellationToken cancellationToken = default)
        {
            if (!Uri.TryCreate(dataSource, UriKind.Absolute, out var uri))
            {
                return BadRequest();
            }

            if (!decimal.TryParse(price, System.Globalization.NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out var priceAmount))
            {
                return BadRequest();
            }

            var beersWithExactPrice = await _beerAggregate.GetByExactPriceAsync(uri, Price.CreateFromDecimal(priceAmount), cancellationToken);

            if (beersWithExactPrice is null || !beersWithExactPrice.Any())
            {
                return NoContent();
            }

            return Ok(beersWithExactPrice.ToBeerFlattenOrdered());
        }

        [HttpGet("getAllCombined")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                    nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> GetAllCombinedAsync([FromQuery] string dataSource, [FromQuery] string price = "17,99", CancellationToken cancellationToken = default)
        {
            if (!Uri.TryCreate(dataSource, UriKind.Absolute, out var uri))
            {
                return BadRequest();
            }

            if (!decimal.TryParse(price, System.Globalization.NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out var priceAmount))
            {
                return BadRequest();
            }

            var beersWithExactPriceTask = _beerAggregate.GetByExactPriceAsync(uri, Price.CreateFromDecimal(priceAmount), cancellationToken);

            var mostBottlesTask = _beerAggregate.GetContainsMostBottlesAsync(uri, cancellationToken);

            var mostExpensiveCheapestTask = _beerAggregate.GetMostExpensiveAndCheapestAsync(uri, cancellationToken);

            await Task.WhenAll(mostExpensiveCheapestTask, mostBottlesTask, beersWithExactPriceTask);

            var beersWithExactPrice = await beersWithExactPriceTask;
            var mostBottles = await mostBottlesTask;
            var (mostExpensive, cheapest) = await mostExpensiveCheapestTask;

            if (beersWithExactPrice is null && mostBottles is null && mostExpensive is null && cheapest is null)
            {
                return NoContent();
            }

            return Ok(new { mostBottles, mostExpensive, cheapest, exactPrice = beersWithExactPrice.ToBeerFlattenOrdered() });
        }
    }
}

