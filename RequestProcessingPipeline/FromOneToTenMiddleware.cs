namespace RequestProcessingPipeline
{
    public class FromOneToTenMiddleware
    {
        private readonly RequestDelegate _next;

        public FromOneToTenMiddleware(RequestDelegate next)
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
                string[] Ones = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };


                if (number == 10)
                {
                   
                    await context.Response.WriteAsync("Your number is ten");
               
                }

                else if (number < 10 && number > 0)
                {
                    
                    await context.Response.WriteAsync("Your number is ten Ones[number % 10 - 2]");
                }

                else if (number % 10 > 0)
                {

                    context.Session.SetString("number", Ones[number % 10 - 1]);
                }
                else if (number % 10 == 0 && number > 20)
                {
                    context.Session.SetString("number", "ten");
                }
            }



            catch (Exception ex)
            {
                // Выдаем окончательный ответ клиенту
                await context.Response.WriteAsync("Incorrect parameter 1-10" + ex.Message);
            }
        }
    }
}
