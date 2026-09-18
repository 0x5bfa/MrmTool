using MrmLib;
using CommunityToolkit.Mvvm.ComponentModel;
using MrmTool.Common;
using Windows.Storage.Streams;

namespace MrmTool.Models
{
    public partial class CandidateItem : ObservableObject
    {
        public CandidateItem(ResourceCandidate candidate)
        {
            Candidate = candidate;
            ValueType = candidate.ValueType;
            StringValue = candidate.StringValue;
            DataValue = candidate.DataValue;
            DataValueBuffer = candidate.DataValueBuffer;
            CandidateQualifiers = candidate.Qualifiers;
        }

        public ResourceCandidate Candidate { get; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Type))]
        [NotifyPropertyChangedFor(nameof(IsExportable))]
        [NotifyPropertyChangedFor(nameof(IsPathCandidate))]
        public partial ResourceValueType ValueType { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Type))]
        [NotifyPropertyChangedFor(nameof(ValueType))]
        [NotifyPropertyChangedFor(nameof(IsExportable))]
        [NotifyPropertyChangedFor(nameof(IsPathCandidate))]
        public partial string StringValue { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(DataValueBuffer))]
        [NotifyPropertyChangedFor(nameof(Type))]
        [NotifyPropertyChangedFor(nameof(ValueType))]
        [NotifyPropertyChangedFor(nameof(IsExportable))]
        [NotifyPropertyChangedFor(nameof(IsPathCandidate))]
        public partial byte[] DataValue { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(DataValue))]
        [NotifyPropertyChangedFor(nameof(Type))]
        [NotifyPropertyChangedFor(nameof(ValueType))]
        [NotifyPropertyChangedFor(nameof(IsExportable))]
        [NotifyPropertyChangedFor(nameof(IsPathCandidate))]
        public partial IBuffer DataValueBuffer { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Qualifiers))]
        public partial IReadOnlyList<Qualifier> CandidateQualifiers { get; set; }

        public string Type => ValueType switch
        {
            ResourceValueType.String => "String",
            ResourceValueType.Path => "File Path",
            ResourceValueType.EmbeddedData => "Embedded Data",
            _ => "Unknown",
        };

        // TODO: Support custom operators and single-operand qualifiers (are these even used anywhere?)
        public string Qualifiers => CandidateQualifiers.Count is 0 ? "(None)" : string.Join(", ",
            CandidateQualifiers.Select(q => $"({q.AttributeName} {q.Operator.Symbol} {q.Value}{(q.Priority is { } p && p != 0 ? $", Priority = {p}" : string.Empty)}{(q.FallbackScore is { } s && s != 0 ? $", Fallback Score = {s}" : string.Empty)})"));

        public bool IsExportable => ValueType is not ResourceValueType.Path;

        public bool IsPathCandidate => ValueType is ResourceValueType.Path;

        partial void OnValueTypeChanged(ResourceValueType value)
        {
            Candidate.ValueType = value;
        }

        partial void OnStringValueChanged(string value)
        {
            Candidate.StringValue = value;
            if (ValueType != Candidate.ValueType)
            {
                ValueType = Candidate.ValueType;
            }
        }

        partial void OnDataValueChanged(byte[] value)
        {
            Candidate.DataValue = value;
            if (!ReferenceEquals(DataValueBuffer, Candidate.DataValueBuffer))
            {
                DataValueBuffer = Candidate.DataValueBuffer;
            }

            if (ValueType != Candidate.ValueType)
            {
                ValueType = Candidate.ValueType;
            }
        }

        partial void OnDataValueBufferChanged(IBuffer value)
        {
            Candidate.DataValueBuffer = value;
            if (!ReferenceEquals(DataValue, Candidate.DataValue))
            {
                DataValue = Candidate.DataValue;
            }

            if (ValueType != Candidate.ValueType)
            {
                ValueType = Candidate.ValueType;
            }
        }

        partial void OnCandidateQualifiersChanged(IReadOnlyList<Qualifier> value)
        {
            Candidate.Qualifiers = value;
        }

        public void SetValue(string value)
        {
            Candidate.SetValue(value);
            ValueType = Candidate.ValueType;
            StringValue = Candidate.StringValue;
        }

        public void SetValue(byte[] value)
        {
            Candidate.SetValue(value);
            ValueType = Candidate.ValueType;
            DataValue = Candidate.DataValue;
            DataValueBuffer = Candidate.DataValueBuffer;
        }

        public void SetValue(IBuffer value)
        {
            Candidate.SetValue(value);
            ValueType = Candidate.ValueType;
            DataValue = Candidate.DataValue;
            DataValueBuffer = Candidate.DataValueBuffer;
        }

        public void SetValue(ResourceValueType valueType, string value)
        {
            Candidate.SetValue(valueType, value);
            ValueType = Candidate.ValueType;
            StringValue = Candidate.StringValue;
        }

        public static implicit operator CandidateItem(ResourceCandidate candidate) => new(candidate);
        public static implicit operator ResourceCandidate(CandidateItem item) => item.Candidate;
    }
}
