namespace KalanMoney.Domain.Entities.ValueObjects;

public record TimeStamp(long Value)
{
    public DateTime ToDateTime()
    {
        var dateTime = new DateTime(1970,1,1,0,0,0,0,System.DateTimeKind.Utc);
        var systemTime = dateTime.AddMilliseconds(Value);

        return systemTime;
    }
    
    public static TimeStamp CreateNow()
    {
        var timeStampValue = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        return new TimeStamp(timeStampValue);
    }
}