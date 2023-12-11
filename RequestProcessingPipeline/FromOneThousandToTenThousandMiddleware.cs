namespace RequestProcessingPipeline
{
    public class FromOneThousandToTenThousandMiddleware
    {
        readonly RequestDelegate _next;
        public FromOneThousandToTenThousandMiddleware(RequestDelegate next)  //next ссылается на следующий компонент
        {
            this._next = next;  //cсылка на следующий компонент , чтоб можно было сделать _next.Invoke() - передать дальше.
        }
        public async Task Invoke(HttpContext context) //контекст запроса  ( адресная строка) 
        {

            string? token = context.Request.Query["number"];   //c контекста получаем параметр number

            try
            {
                string[] nums = { "one", "two", "three", "four", "fife", "six", "seven", "eight", "nine" , "ten" };
                int number = Convert.ToInt32(token);
                number = Math.Abs(number);
               

                if (number >= 1000 && number <= 10999)
                {
                    if (number%1000 == 0)
                    {
                        await context.Response.WriteAsync($"Your number is {nums[number / 1000 -1]} thousand");
                    }
                    else
                    {
                        await _next.Invoke(context);
                        string? result = string.Empty;
                        result = context.Session.GetString("number");
                        await context.Response.WriteAsync($"Your number is {nums[number / 1000 - 1]} thousand {result}");
                    }
                }
                else 
                {
                    await _next.Invoke(context);
                }
               



            }
            catch (Exception)
            {
                await context.Response.WriteAsync("Incorrect parametr 1000-9999");
            }

        }
    }
}
