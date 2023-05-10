using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace officehours_management.Data.Static
{
    public class OfficeHourDay
    {
        public static IList<SelectListItem> Day = new List<SelectListItem>
        {
            new SelectListItem{ Text="Saturday", Value="Saturday", Selected=true },
            new SelectListItem{ Text="Sunday", Value="Sunday" },
            new SelectListItem{ Text="Monday", Value="Monday", },
            new SelectListItem{ Text="Tuesday", Value="Tuesday", },
            new SelectListItem{ Text="Wednesday", Value="Wednesday", },
            new SelectListItem{ Text="Thursday", Value="Thursday", },
            new SelectListItem{ Text="Friday", Value="Friday", }
        }.ToImmutableArray();

    }
}