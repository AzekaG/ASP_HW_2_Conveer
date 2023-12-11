namespace RequestProcessingPipeline
{
    public class FromElevenThousandToTwentyThousandMiddleware
    {
        readonly RequestDelegate _next;
        public FromElevenThousandToTwentyThousandMiddleware(RequestDelegate next)  //next ссылается на следующий компонент
        {
            this._next = next;  //cсылка на следующий компонент , чтоб можно было сделать _next.Invoke() - передать дальше.
        }
        public async Task Invoke(HttpContext context) //контекст запроса  ( адресная строка) 
        {

            string? token = context.Request.Query["number"];   //c контекста получаем параметр number

            try
            {
                string[] nums = {"eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eightteen", "ninteen" };
                int number = Convert.ToInt32(token);
                number = Math.Abs(number); // 
                if (number > 10999 && number <=19999)
                {
                    if (number % 1000 == 0)
                    {
                        await context.Response.WriteAsync($"Your number is {nums[number / 1000 % 10 - 1]} thousand");

                    }

                    else
                    {
                        await _next.Invoke(context);
                        string? result = string.Empty; 
                        result = context.Session.GetString("number");
                        await context.Response.WriteAsync($"Your number is {nums[number / 1000 % 10 - 1]} thousand {result}");
                    }
                }
                else if(number < 11000)
                {
                    await _next.Invoke(context);
                }
                else if(number > 20000)
                {
                    await _next.Invoke(context);
                }


            }
            catch (Exception)
            {
                await context.Response.WriteAsync("Incorrect parametr 11000-19999");
            }

        }
    }
}
