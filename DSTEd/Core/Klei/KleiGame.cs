namespace DSTEd.Core.Klei {
    class KleiGame {
        protected int id = -1;
        protected string name = null;
        protected string path = null;

        public string GetName() {
            return this.name;
        }

        public int GetID() {
            return this.id;
        }

        public void SetPath(string path) {
            this.path = path;
        }

        public string GetPath() {
            return this.path;
        }
    }
}
