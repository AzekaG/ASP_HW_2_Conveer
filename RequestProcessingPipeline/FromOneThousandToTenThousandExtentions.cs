namespace RequestProcessingPipeline
{
    public static class FromOneThousandToTenThousandExtentions
    {
        public static IApplicationBuilder UseFromOneThousandToTenThousand(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FromOneThousandToTenThousandMiddleware>();
        }
    }
}
