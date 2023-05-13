namespace Feast.Foundation.Algorithm
{
    public abstract class DecisionNodeBase<TSample, TState>
    {
        internal virtual void Reset() => Route = -1;
        protected int Route { get; set; } = -1;
        public required TState State { get; init; }
        public List<DecisionNode<TSample, TState>> Nodes { get; init; } = new();
        public bool IsLeaf => Nodes.Count == 0;
    }

    public sealed class DecisionNode<TSample, TState> : DecisionNodeBase<TSample, TState>
    {
        public readonly Func<IReadOnlyList<TSample>, TSample, bool> Condition;
        public DecisionNode(Func<IReadOnlyList<TSample>, TSample, bool> condition) => Condition = condition;

        #region Private
        internal override void Reset()
        {
            if (Route >= 0) { Nodes[Route].Reset(); }
            base.Reset();
        }
        private TState VisitNode(IReadOnlyList<TSample> tree,
            TSample sample,
            out bool finished) => Nodes[Route].Visit(tree, sample, out finished);
        #endregion

        public TState Visit(IReadOnlyList<TSample> tree, TSample sample, out bool finished)
        {
            if (IsLeaf)
            {
                finished = true;
                return State;
            }

            if (Route >= 0)
            {
                return VisitNode(tree, sample, out finished);
            }

            for (var i = 0; i < Nodes.Count; i++)
            {
                if (!Nodes[i].Condition(tree, sample)) continue;
                Route = i;
                return VisitNode(tree, sample, out finished);
            }

            finished = false;
            return State;
        }
    }

    public sealed class DecisionTree<TSample, TState> : DecisionNodeBase<TSample, TState>
    {
        public IReadOnlyList<TSample> Samples => samples;
        public delegate void FinishHandler(DecisionTree<TSample,TState> tree,TState final);
        public event FinishHandler? Finished;

        #region Private
        private readonly List<TSample> samples = new();
        private TState VisitNode(TSample sample)
        {
            var res = Nodes[Route].Visit(Samples, sample, out var fin);
            if (!fin) return res;
            Finished?.Invoke(this, res);
            Reset();
            return res;
        }
        internal override void Reset()
        {
            if(Route  >= 0) Nodes[Route].Reset();
            base.Reset();
            samples.Clear();
        }
        #endregion

        public TState Visit(TSample sample)
        {
            samples.Add(sample);
            if (IsLeaf)
            {
                Reset();
                return State;
            }

            if (Route >= 0)
            {
                return VisitNode(sample);
            }

            for (var i = 0; i < Nodes.Count; i++)
            {
                if (!Nodes[i].Condition(Samples, sample)) continue;
                Route = i;
                return VisitNode(sample);
            }

            return State;
        }

    }

}
