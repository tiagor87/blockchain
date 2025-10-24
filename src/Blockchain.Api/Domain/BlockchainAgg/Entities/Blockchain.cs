using System.Collections;

namespace Blockchain.Api.Domain.BlockchainAgg.Entities;

public class Blockchain : IEnumerable<Block>
{   
    public IEnumerator<Block> GetEnumerator()
    {
        return new BlockchainIterator(Last);
    }

    public Block Last { get; private set; } = Block.Genesis;
    
    public Block Create()
    {
        var block = Block.Create(Last);
        Last = block;
        return block;
    }
    
    public object ToView()
    {
        return new
        {
            length = this.Count(),
            chain = this.Select(block => block.ToView())
        };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    class BlockchainIterator : IEnumerator<Block>
    {
        private Block _last;
        private Block? _current;

        public BlockchainIterator(Block last)
        {
            _last = last ?? throw new ArgumentNullException(nameof(last));
        }

        public bool MoveNext()
        {
            if (_current is null)
            {
                _current = _last;
                return true;
            }
        
            if (_current == Block.Genesis)
            {
                return false;
            }
        
            _current = _current.Previous;
            return true;
        }

        public void Reset()
        {
            _current = null;
        }

        Block IEnumerator<Block>.Current => _current ?? throw new InvalidOperationException();

        object? IEnumerator.Current => _current;

        public void Dispose()
        {
            // Do nothing
        }
    }
}