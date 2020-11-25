using System.Security.Permissions;

namespace WebAppExample.Services
{
    public class Counter
    {
        private int _count;

        public int Count => _count;

        public void MoveCounter()
        {
            _count++;
        }
    }
}