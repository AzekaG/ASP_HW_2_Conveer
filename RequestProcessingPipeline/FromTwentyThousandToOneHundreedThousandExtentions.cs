namespace RequestProcessingPipeline
{
    public static class FromTwentyThousandToOneHundreedThousandExtentions
    {
                       
        public static IApplicationBuilder UseFromTwentyThousandToOneHundreedThousand(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FromTwentyThousandToOneHundreedThousandMiddleware>();
        }
    }
}
