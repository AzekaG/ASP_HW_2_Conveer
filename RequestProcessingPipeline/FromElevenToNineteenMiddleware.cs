﻿namespace RequestProcessingPipeline
{
    public class FromElevenToNineteenMiddleware
    {
        private readonly RequestDelegate _next;

        public FromElevenToNineteenMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string? token = context.Request.Query["number"];
            try
            {
                int number = Convert.ToInt32(token);
                number = Math.Abs(number);

                if (number%100 < 11 || number%100 > 19)
                {
                    await _next.Invoke(context);  //Контекст запроса передаем следующему компоненту
                }

                else
                {
                    string[] Numbers = { "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                    // Выдаем окончательный ответ клиенту
                    if (number > 100)
                    {
                        context.Session.SetString("number", Numbers[number % 10 - 1]);
                    }
                    else
                    {
                        await context.Response.WriteAsync("Your number is " + Numbers[number - 11]);
                    }
                }
            }
            catch (Exception)
            {
                // Выдаем окончательный ответ клиенту
                await context.Response.WriteAsync("Incorrect parameter 11-19");
            }
        }
    }
}
