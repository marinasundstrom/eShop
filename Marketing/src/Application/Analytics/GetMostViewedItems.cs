using MediatR;

using Microsoft.EntityFrameworkCore;
using YourBrand.Marketing.Domain;

namespace YourBrand.Marketing.Application.Analytics.Commands;


public record GetMostViewedItems(DateTime? From = null, DateTime? To = null) : IRequest<Data>
{
    public class Handler : IRequestHandler<GetMostViewedItems, Data>
    {
        private readonly IApplicationDbContext context;

        public Handler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Data> Handle(GetMostViewedItems request, CancellationToken cancellationToken)
        {
             var events = await context.Events
                        .Where(x => x.EventType == YourBrand.Marketing.Domain.Enums.EventType.PageViewed)
                        .OrderBy(x => x.DateTime)
                        .AsNoTracking()
                        .AsSplitQuery()
                        .ToListAsync();

            List<DateTime> months = new();

            const int monthSpan = 5;

            DateTime lastDate = request.To?.Date ?? DateTime.Now.Date;
            DateTime firstDate = request.From?.Date ?? lastDate.AddMonths(-monthSpan)!;

            for (DateTime dt = firstDate; dt <= lastDate; dt = dt.AddMonths(1))
            {
                months.Add(dt);
            }

            List<Series> series = new();

            var firstMonth = DateOnly.FromDateTime(firstDate);
            var lastMonth = DateOnly.FromDateTime(lastDate);

            foreach (var itemGroup in events.GroupBy(x => x.Data).Take(10))
            {
                var keyObj = System.Text.Json.JsonDocument.Parse(itemGroup.Key);

                List<decimal> values = new();

                foreach (var month in months)
                {
                    //var value = itemGroup
                    //    .Where(e => e.DateTime.Year == month.Year && e.DateTime.Month == month.Month)
                    //    .Count();

                    var value = itemGroup
                            .Where(e => e.DateTime.Year == month.Year && e.DateTime.Month == month.Month)
                            .DistinctBy(x => x.ClientId)
                            .Count();

                    values.Add((decimal)value);
                }

                series.Add(new Series(keyObj.RootElement.GetProperty("ItemId").GetString()!, values));
            }

            return new Data(
                months.Select(d => d.ToString("MMM yy")).ToArray(),
                series);
        }
    }
}
