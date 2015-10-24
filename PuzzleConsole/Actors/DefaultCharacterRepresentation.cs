[System.AttributeUsage(System.AttributeTargets.Class |
                       System.AttributeTargets.Struct)
]
public class DefaultCharacterRepresentation : System.Attribute
{
    public string character;

    public DefaultCharacterRepresentation(string character)
    {
        this.character = character;
    }
}