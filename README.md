# VL.Postman

*A VL plugin to turn Postman collection exports into ready to use nodes.*

## What

When implementing a REST API in vvvv, you'd usually first want to test it in Postman to see how it behaves. This allows you to get familiar with parameters, potential error messages, etc.

It's also a good idea to properly structure your queries using folders and add a brief documentation there to explain what they do.

But then comes the time where you have to patch all those queries in vvvv, which is kind of boring since you've already done the job once...

Here comes VL.Postman : simply export your Collection as JSON, put it in a `postman` folder next to your patch and let the plugin create the nodes for you!

## How

Under the hood, the nodes use the RestSharp library. For each node, you get a RestResponse output that you can then use with all the nodes the nuget provides.
