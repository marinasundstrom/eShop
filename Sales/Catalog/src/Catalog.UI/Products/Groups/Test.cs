using Microsoft.AspNetCore.Components;

using MudBlazor;

namespace YourBrand.Catalog.Products;

public static class Test
{
    public static ProductGroupTreeNodeDto? FindNode(this IEnumerable<ProductGroupTreeNodeDto> nodes, long groupId)
    {
        ProductGroupTreeNodeDto? productGroupTreeNode = null;

        foreach (var childNode in nodes)
        {
            productGroupTreeNode = childNode.FindNode(groupId);

            if (productGroupTreeNode is not null)
                break;
        }

        return productGroupTreeNode;
    }

    public static ProductGroupTreeNodeDto? FindNode(this IEnumerable<ProductGroupTreeNodeDto> nodes, string path)
    {
        ProductGroupTreeNodeDto? productGroupTreeNode = null;

        foreach (var childNode in nodes)
        {
            productGroupTreeNode = childNode.FindNode(path);

            if (productGroupTreeNode is not null)
                break;
        }

        return productGroupTreeNode;
    }

    public static ProductGroupTreeNodeDto? FindNode(this ProductGroupTreeNodeDto productGroupTreeNode, long groupId)
    {
        if (productGroupTreeNode.Id == groupId)
        {
            return productGroupTreeNode;
        }

        foreach (var childNode in productGroupTreeNode.SubGroups)
        {
            var r = FindNode(childNode, groupId);

            if (r is not null)
                return r;
        }

        return null;
    }

    public static ProductGroupTreeNodeDto? FindNode(this ProductGroupTreeNodeDto productGroupTreeNode, string path)
    {
        if (productGroupTreeNode.Path.StartsWith(path))
        {
            return productGroupTreeNode;
        }

        foreach (var childNode in productGroupTreeNode.SubGroups)
        {
            var r = FindNode(childNode, path);

            if (r is not null)
                return r;
        }

        return null;
    }
}
