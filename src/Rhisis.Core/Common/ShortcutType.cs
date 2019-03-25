namespace Rhisis.Core.Common
{
    public enum ShortcutType : uint
    {
        None,
        Etc,
        Applet,
        Motion,
        Script,
        Item,
        Skill,
        Object,
        Chat,
        SkillFun,
        Emoticon,
        LordSkill
    }

    public enum ShortcutObjectType : uint
    {
        Item,
        Card,
        Cube,
        Pet
    }

    public enum ShortcutTaskbarTarget
    {
        Applet,
        Item,
        Queue
    }
}