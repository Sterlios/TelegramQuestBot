public class Reward
{
    public string Text { get; set; }

    public Reward ToCopy()
    {
        Reward reward = new Reward();
        reward.Text = Text;

        return reward;
    }
}