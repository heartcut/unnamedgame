namespace pythonbackendgame
{
    public class SoundService
    {
        public event Func<Task> OnPlaySound;

        public async Task PlaySound()
        {
            if (OnPlaySound != null)
            {
                await OnPlaySound.Invoke();
            }
        }
    }
}
