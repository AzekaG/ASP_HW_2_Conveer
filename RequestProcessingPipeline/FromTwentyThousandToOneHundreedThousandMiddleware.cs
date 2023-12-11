namespace RequestProcessingPipeline
{
    public class FromTwentyThousandToOneHundreedThousandMiddleware
    {

        readonly RequestDelegate _next;


        public FromTwentyThousandToOneHundreedThousandMiddleware(RequestDelegate next)  //next ссылается на следующий компонент
        {
            this._next = next;  //cсылка на следующий компонент , чтоб можно было сделать _next.Invoke() - передать дальше.
        }


        public async Task Invoke(HttpContext context) //контекст запроса  ( адресная строка) 
        {

            string? token = context.Request.Query["number"];   //c контекста получаем параметр number

            try
            {
                string[] nums = { "twenty", "thirty", "fourty", "fifty", "sixty", "seventy", "eighty", "ninety" };
                string[] numsSimple = { "one", "two", "three", "four", "fife", "six", "seven", "eight", "nine" };
                int number = Convert.ToInt32(token);
                number = Math.Abs(number); 
                if (number > 100000)
                {
                    await context.Response.WriteAsync("Number is greater then one hundred thousand"); // условиям не подходит , поэтому выдает ответ сразу.

                }
                else if (number == 100000) await context.Response.WriteAsync($"Your number is one hundred thousand");
                else if (number >= 20000 && number <=99999)
                {
                    if (number % 10000 == 0)
                    {
                        await context.Response.WriteAsync($"Your number is {nums[number / 10000 - 2]} thousand");
                    }
                    else if(number / 1000 %10 != 0 && number% 1000 == 0)
                    {
                        await context.Response.WriteAsync($"Your number is {nums[number / 10000 - 2]} {numsSimple[number/1000%10-1]} thousand");
                    }
                    else
                    {
                        await _next.Invoke(context);
                        string? result = string.Empty;
                        result = context.Session.GetString("number"); //получаем число от компонента fromonetotenmiddleware
                        await context.Response.WriteAsync($"Your number is {nums[number / 10000 - 2]} {numsSimple[number / 1000 % 10 - 1]} thousand {result}");//окончательный результат
                    }
                }
                else
                {
                     await _next.Invoke(context);

                    
                   
                }
               
               
            }
            catch (Exception ex)
            {
                await context.Response.WriteAsync("Incorrect parametr 20000-99999" + ex.Message);
            }

        }








    }
}
