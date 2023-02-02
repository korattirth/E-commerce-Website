using E_commerce_Website.DTOs;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace E_commerce_Website.Utils
{
    public class WeatherForecastExamples : IExamplesProvider<WeatherForecast>
    {
        public WeatherForecast GetExamples()
        {
            return new WeatherForecast()
            {
                Date = DateTime.Now.Date,
                TemperatureC = 50,
                Summary = "test data"
            };
        }
    }
}
