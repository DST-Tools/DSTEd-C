using System;

namespace DSTEd.Core.Steam {
    class Account {
        public void Login(Action callback) {
            callback();
        }

        public void Logout(Action callback) {
            callback();
        }
    }
}
