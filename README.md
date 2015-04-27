**heisenberg** is a simple plot creator, bundled with a couple of instruction to try and shed a little light on "heisenberg's uncertainty principle".

# How it works:

**1)** As soon as it opens up, you will be able to do 3 things:
	Press (+) to create a new plot;
	Press (H) to open up a simple explanation with examples;
	Slide the dot at the top-right corner of the screen to change the number of measures of each plot;

**2)** If you choose (+) you will be asked to provide a standard deviation and a mean for any of the 3 graphs:
''
	<p>- A(k) &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&rArr;&nbsp; y = e^{-0.5*[(x-stDev)/mean]^ 2} </p>
	<p>- Re(&Psi;(x)) &nbsp;&rArr;&nbsp; y = 2 * mean * sqrt(&Pi;) * e^[-1 * (mean * x)^2] * cos(stdDev * x) </p>
	<p>- |&Psi;(x)|^2 &nbsp;&rArr;&nbsp; y = 4 * mean^2 * &Pi; * e^[2*(mean*x)^2] </p>

**3)** If you choose (H) a screen like below will popup (the explanation is currently in Portuguese):
	<p><img src="/Screenshots/Explanation.png" alt="Explanation.png"></p>

# Scaling:

All the scaling is done independently on each type of graph (not on each graph!) and automatically. According to the user-provided value for the standard deviance, an "interesting" range of values is calculated and only that range will be ploted.

To give a simple example of all this, here is a screenshot with 3 different values of standard deviation and, for each of those values, all three types of graph are plotted in a row.
	<p><img src="/Screenshots/Example.png" alt="Example.png"></p>

# Other features:

**1)** You can close a graph by clicking the button at its top left corner.

**2)** You can freely move any graph by clicking on top of it and dragging.