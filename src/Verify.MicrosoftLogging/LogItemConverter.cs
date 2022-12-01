class LogItemConverter :
    WriteOnlyJsonConverter<LogItem>
{
    public override void Write(VerifyJsonWriter writer, LogItem item)
    {
        writer.WriteStartObject();
        writer.WritePropertyName(item.Level.ToString());
        writer.WriteRawValueIfNoStrict(item.Message);
        writer.WriteMember(item, item.Category, "Category");
        if (item.EventId.Id != 0)
        {
            writer.WriteMember(item, item.EventId, "EventId");
        }
        writer.WriteMember(item, item.State, "State");
        writer.WriteMember(item, item.Exception, "Exception");
        writer.WriteEndObject();
    }
}