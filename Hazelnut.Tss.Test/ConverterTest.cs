namespace Hazelnut.Tss.Test;

[TestClass]
public sealed class ConverterTest
{
    [TestMethod]
    public void ConverterTest1()
    {
        var converter = new AnsiCodeConverter();
        var result = converter.Convert("\e[35mHello, world!\e[0m");
        
        Assert.AreEqual("<span style=\"color: #800080;\">Hello, world!</span>", result);
    }

    [TestMethod]
    public void ConverterTest2()
    {
        var converter = new AnsiCodeConverter();
        var result = converter.Convert("\e]8;;https://daram.in\e\x9c\e[35mHello, world!\e[0m");
        
        Assert.AreEqual("<a href=\"https://daram.in\"><span style=\"color: #800080;\">Hello, world!</span></a>", result);
    }

    [TestMethod]
    public void ConverterTest3()
    {
        var converter = new AnsiCodeConverter();
        var result = converter.Convert("\e]8;;https://daram.in\e\x9c\e[35mHello, world!\e[0mSample");
        
        Assert.AreEqual("<a href=\"https://daram.in\"><span style=\"color: #800080;\">Hello, world!</span></a><a href=\"https://daram.in\"><span style=\"\">Sample</span></a>", result);
    }

    [TestMethod]
    public void ConverterTest4()
    {
        var converter = new AnsiCodeConverter();
        var result = converter.Convert("\e]8;;https://daram.in\e\x9c\e[35mHello, world!\e]8;;\e\x9c\e[0mSample");
        
        Assert.AreEqual("<a href=\"https://daram.in\"><span style=\"color: #800080;\">Hello, world!</span></a>Sample", result);
    }

    [TestMethod]
    public void ConverterTest5()
    {
        var converter = new AnsiCodeConverter();
        var result = converter.Convert("\e]8;;https://daram.in\e\x9c\e[35mHello, world!\e[0m\e]8;;\e\x9cSample");
        
        Assert.AreEqual("<a href=\"https://daram.in\"><span style=\"color: #800080;\">Hello, world!</span></a>Sample", result);
    }
}