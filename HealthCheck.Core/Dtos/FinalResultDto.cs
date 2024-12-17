namespace HealthCheck.Core.Dtos;

public class FinalResultDto
{
    public string CategoryTitleEn { get; set; } = null!;
    public string CategoryTitleFi { get; set; } = null!;
    public int GoodAnswerCount { get; set; }
    public int NeutralAnswerCount { get; set; }
    public int BadAnswerCount { get; set; }
    public int AverageAnswer { get; set; }
}
