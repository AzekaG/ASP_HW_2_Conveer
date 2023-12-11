using Microsoft.AspNetCore.Http;

namespace RequestProcessingPipeline
{
    public class FromHundredToThousandMiddleware
    {
        readonly RequestDelegate _next;
        public FromHundredToThousandMiddleware(RequestDelegate next)  //next ссылается на следующий компонент
        {
            this._next = next;  //cсылка на следующий компонент , чтоб можно было сделать _next.Invoke() - передать дальше.
        }
        public async Task Invoke(HttpContext context) //контекст запроса  ( адресная строка) 
        {

            string? token = context.Request.Query["number"];   //c контекста получаем параметр number

            try
            {
                string[] nums = { "one", "two", "three", "four", "fife", "six", "seven", "eight", "nine" };
                int number = Convert.ToInt32(token);
                number = Math.Abs(number);



                if (number >= 100 && number <= 999)
                {
                    if (number % 100 == 0)
                    {
                       
                        await context.Response.WriteAsync($"Your number is {nums[number / 100 - 1]} hundred");

                    }
                    
                    else
                    {
                        
                        await _next.Invoke(context);
                        string? result = string.Empty;
                        result = context.Session.GetString("number");
                        await context.Response.WriteAsync($"Your number is {nums[number / 100 - 1]} hundred {result}");

                    }
                }
                else if(number < 100)
                {
                    
                    await _next.Invoke(context);
                }
                else if(number > 999)
                {

                    if (number % 100 > 0 && number % 100 <= 99)
                    {


                        await _next.Invoke(context);
                        string? result = string.Empty;
                        result = context.Session.GetString("number");

                        if (number % 1000 / 100 == 0)
                        {
                            context.Session.SetString("number", $" {result}");
                        }
                        else
                        {

                            context.Session.SetString("number", $" {nums[number / 100 % 10 - 1]} hundred {result}");

                        }


                    }
                    else if( number %100 == 0)
                    {
                        context.Session.SetString("number", $" {nums[number / 100 % 10 - 1]} hundred");
                    }





                }


            }
            catch (Exception ex)
            {
                await context.Response.WriteAsync("Incorrect parametr 100-999"  +ex.Message);
            }

        }










    }
}
