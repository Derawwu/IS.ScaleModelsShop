using IS.ScaleModelsShop.Application.Responses;

namespace IS.ScaleModelsShop.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandResponse : BaseResponse
    {
        public CreateCategoryCommandResponse() : base()
        {

        }

        public CreateCategoryDTO Category { get; set; } = default!;
    }
}
