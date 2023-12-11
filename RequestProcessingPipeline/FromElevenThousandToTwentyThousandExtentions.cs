namespace RequestProcessingPipeline
{
    public static class FromElevenThousandToTwentyThousandExtentions
    {
        public static IApplicationBuilder UseFromElevenThousandToTwentyThousand(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FromElevenThousandToTwentyThousandMiddleware>();
        }
    }
}
