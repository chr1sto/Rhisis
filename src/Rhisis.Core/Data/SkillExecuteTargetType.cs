namespace Rhisis.Core.Data
{
    /// <summary>
    /// Defines the differents skill executation targets.
    /// </summary>
    public enum SkillExecuteTargetType
    {
        /// <summary>
        /// No target.
        /// </summary>
        None,

        /// <summary>
        /// Executes the skill only on the skill caster to change a parameter.
        /// </summary>
        SelfChangeParameter,

        /// <summary>
        /// Executes the skill only on the skill caster to change a parameter.
        /// </summary>
        ObjectChangeParamter,

        /// <summary>
        /// Can execute magic projectile on a given target.
        /// </summary>
        /// <remarks>
        /// This target doesn't attack the target.
        /// </remarks>
        MagicShot,
        
        /// <summary>
        /// Can execute an instant magic attack on a given target.
        /// </summary>
        MagicAttack,
        
        /// <summary>
        /// Not used.
        /// </summary>
        Amplification,
        
        /// <summary>
        /// Not used.
        /// </summary>
        Attacker,
        
        /// <summary>
        /// Can execute a general magic attack on a given target.
        /// </summary>
        Magic,

        /// <summary>
        /// Can execute the skill only on the target.
        /// </summary>
        Another,

        /// <summary>
        /// Can execute the skill only on the skill caster and other targets.
        /// </summary>
        AnotherWith,

        /// <summary>
        /// Not used.
        /// </summary>
        Summon,

        /// <summary>
        /// Not used.
        /// </summary>
        AroundAttack,

        /// <summary>
        /// Not used.
        /// </summary>
        Other,

        /// <summary>
        /// Can execute the skill only on members of the troupe (group/party).
        /// </summary>
        Troupe,

        /// <summary>
        /// Can execute a magic *projectile* attack shot on a given target.
        /// </summary>
        /// <remarks>
        /// Mostly used for magic ranged attacks like magician attacks.
        /// </remarks>
        MagicAttackShot,

        /// <summary>
        /// Not used.
        /// </summary>
        MentalAttack,

        /// <summary>
        /// Not used.
        /// </summary>
        MeleeAttackShot,
        
        /// <summary>
        /// Can execute a melee attack on a given target.
        /// </summary>
        MeleeAttack,
        
        /// <summary>
        /// Not used.
        /// </summary>
        RangeAttack,
        
        /// <summary>
        /// Not used.
        /// </summary>
        Pet,

        /// <summary>
        /// Can execute the skill only on members of the troupe (group/party) and the caster itself.
        /// </summary>
        TroupeWith,
        
        /// <summary>
        /// Not used.
        /// </summary>
        Item,
    }
}
