namespace Site.Client.Items.Item;

public class OptionVM
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public OptionType OptionType { get; set; }

        public OptionGroupDto Group { get; set; } = null!;

        public string? ItemId { get; set; }

        public decimal? Price { get; set; }

        public bool IsSelected { get; set; }

        public List<OptionValueVM> Values { get; } = new List<OptionValueVM>();

        public string? SelectedValueId { get; set; }

        public int? MinNumericalValue { get; set; }

        public int? MaxNumericalValue { get; set; }

        public int? NumericalValue { get; set; }

        public string? TextValue { get; set; }

        public int? TextValueMinLength { get; set; }

        public int? TextValueMaxLength { get; set; }
    }
