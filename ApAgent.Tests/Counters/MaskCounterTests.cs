using ApAgent.Counters;

namespace ApAgent.Tests.Counters;

public class MaskCounterTests
{
    private readonly string _testDirectoryPath;

    public MaskCounterTests()
    {
        // Create a temporary test directory
        _testDirectoryPath = Path.Combine(Path.GetTempPath(), "TestDirectory_" + Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testDirectoryPath);
    }

    [Fact]
    public void CountMask_WhenMaskDoesNotExist_ReturnsDirectoryName()
    {
        // Arrange
        var counter = new MaskCounter();

        // Act
        var result = counter.CountMask(_testDirectoryPath);

        // Assert
        Assert.Equal(new DirectoryInfo(_testDirectoryPath).Name, result);
    }

    [Fact]
    public void CountMask_WithDifferentDirectoryNames_ReturnsCorrectMask()
    {
        // Arrange
        var counter = new MaskCounter();
        var testDir = Path.Combine(Path.GetTempPath(), "MyCustomDirectory");
        Directory.CreateDirectory(testDir);

        try
        {
            // Act
            var result = counter.CountMask(testDir);

            // Assert
            Assert.Equal("MyCustomDirectory", result);
        }
        finally
        {
            // Cleanup
            if (Directory.Exists(testDir))
            {
                Directory.Delete(testDir);
            }
        }
    }

    [Fact]
    public void CountMask_WhenMaskExists_AppendsIndexToMask()
    {
        // Arrange
        var dirName = new DirectoryInfo(_testDirectoryPath).Name;
        var counter = new TestMaskCounterWithExistingMask(dirName);

        // Act
        var result = counter.CountMask(_testDirectoryPath);

        // Assert
        // The first mask exists, so it should return the mask with "2" appended
        Assert.Equal(dirName + "2", result);
    }

    [Fact]
    public void CountMask_WhenMultipleMasksExist_ReturnsFirstAvailableIndex()
    {
        // Arrange
        var dirName = new DirectoryInfo(_testDirectoryPath).Name;
        var counter = new TestMaskCounterWithMultipleExistingMasks(new[] { dirName, dirName + "2", dirName + "3" });

        // Act
        var result = counter.CountMask(_testDirectoryPath);

        // Assert
        // The first three masks exist, so it should return the mask with "4" appended
        Assert.Equal(dirName + "4", result);
    }

    [Fact]
    public void CountMask_WhenManyMasksExist_IncrementsCorrectly()
    {
        // Arrange
        var dirName = new DirectoryInfo(_testDirectoryPath).Name;
        var existingMasks = new List<string>();
        for (int i = 0; i < 10; i++)
        {
            existingMasks.Add(i == 0 ? dirName : dirName + (i + 1));
        }
        var counter = new TestMaskCounterWithMultipleExistingMasks(existingMasks.ToArray());

        // Act
        var result = counter.CountMask(_testDirectoryPath);

        // Assert
        Assert.Equal(dirName + "11", result);
    }

    [Fact]
    public void MaskExists_InBaseClass_AlwaysReturnsFalse()
    {
        // Arrange
        var counter = new TestableBaseMaskCounter();

        // Act & Assert
        Assert.False(counter.TestMaskExists("AnyMask"));
        Assert.False(counter.TestMaskExists(""));
        Assert.False(counter.TestMaskExists("TestMask123"));
    }

    // Helper class to test the protected MaskExists method
    private sealed class TestableBaseMaskCounter : MaskCounter
    {
        public bool TestMaskExists(string mask)
        {
            return MaskExists(mask);
        }
    }

    // Helper class that simulates a single existing mask
    private sealed class TestMaskCounterWithExistingMask : MaskCounter
    {
        private readonly string _existingMask;

        public TestMaskCounterWithExistingMask(string existingMask)
        {
            _existingMask = existingMask;
        }

        protected override bool MaskExists(string mask)
        {
            return mask == _existingMask || base.MaskExists(mask);
        }
    }

    // Helper class that simulates multiple existing masks
    private sealed class TestMaskCounterWithMultipleExistingMasks : MaskCounter
    {
        private readonly HashSet<string> _existingMasks;

        public TestMaskCounterWithMultipleExistingMasks(string[] existingMasks)
        {
            _existingMasks = new HashSet<string>(existingMasks);
        }

        protected override bool MaskExists(string mask)
        {
            return _existingMasks.Contains(mask);
        }
    }
}
