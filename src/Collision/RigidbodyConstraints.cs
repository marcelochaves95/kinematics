namespace Kinematics.Collision
{
    /// <summary>
    ///   <para>Use these flags to constrain motion of Rigidbodies.</para>
    /// </summary>
    public enum RigidbodyConstraints
    {
        /// <summary>
        ///   <para>No constraints.</para>
        /// </summary>
        None = 0,
        /// <summary>
        ///   <para>Freeze motion along the X-axis.</para>
        /// </summary>
        FreezePositionX = 2,
        /// <summary>
        ///   <para>Freeze motion along the Y-axis.</para>
        /// </summary>
        FreezePositionY = 4,
        /// <summary>
        ///   <para>Freeze motion along the Z-axis.</para>
        /// </summary>
        FreezePositionZ = 8,
        /// <summary>
        ///   <para>Freeze motion along all axes.</para>
        /// </summary>
        FreezePosition = 14, // 0x0000000E
        /// <summary>
        ///   <para>Freeze rotation along the X-axis.</para>
        /// </summary>
        FreezeRotationX = 16, // 0x00000010
        /// <summary>
        ///   <para>Freeze rotation along the Y-axis.</para>
        /// </summary>
        FreezeRotationY = 32, // 0x00000020
        /// <summary>
        ///   <para>Freeze rotation along the Z-axis.</para>
        /// </summary>
        FreezeRotationZ = 64, // 0x00000040
        /// <summary>
        ///   <para>Freeze rotation along all axes.</para>
        /// </summary>
        FreezeRotation = 112, // 0x00000070
        /// <summary>
        ///   <para>Freeze rotation and motion along all axes.</para>
        /// </summary>
        FreezeAll = 126, // 0x0000007E
    }
}