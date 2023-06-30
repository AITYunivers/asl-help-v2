public partial class Mono : Basic
{
    public override Basic Exit()
    {
        _ = base.Exit();

        return this;
    }

    public override Basic Shutdown()
    {
        _ = base.Shutdown();

        return this;
    }
}
