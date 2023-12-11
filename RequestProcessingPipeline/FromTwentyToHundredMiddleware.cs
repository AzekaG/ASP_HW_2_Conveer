using Microsoft.AspNetCore.Http;

namespace RequestProcessingPipeline
{
    public class FromTwentyToHundredMiddleware
    {
        private readonly RequestDelegate _next;

        public FromTwentyToHundredMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string? token = context.Request.Query["number"]; // Получим число из контекста запроса
            try
            {
                int number = Convert.ToInt32(token);
                number = Math.Abs(number);

                if(number >=20 && number <= 99)
                {
                    string[] Tens = { "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };
                    if (number % 10 == 0)
                    {
                        await context.Response.WriteAsync("Your number is " + Tens[number / 10 - 2]);
                    }
                    else
                    {
                        await _next.Invoke(context); // Контекст запроса передаем следующему компоненту
                        string? result = string.Empty;
                        result = context.Session.GetString("number"); // получим число от компонента FromOneToTenMiddleware
                        await context.Response.WriteAsync($"Your number is {Tens[number / 10 - 2]} {result}");
                    }
                    
                }              
                else if(number < 20)
                {
                    await _next.Invoke(context);
                }
                else if(number > 99)
                {
                    if(number% 100 >= 20 && number%100 <= 99)
                    {
                        string[] Tens = { "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };
                        if (number % 10 == 0)
                        {
                            
                            context.Session.SetString("number", Tens[number/10%10 - 2]);
                        }
                        else
                        {
                            
                            await _next.Invoke(context); // Контекст запроса передаем следующему компоненту
                            string? result = string.Empty;
                            result = context.Session.GetString("number"); // получим число от компонента FromOneToTenMiddleware
                            context.Session.SetString("number", Tens[number / 10%10 - 2] + result);
                            
                        }
                    }
                    else
                    {
                        await _next.Invoke(context);
                    }


                }
            }
            catch (Exception)
            {
                // Выдаем окончательный ответ клиенту
                await context.Response.WriteAsync("Incorrect parameter 20-100");
            }
        }
    }
}
