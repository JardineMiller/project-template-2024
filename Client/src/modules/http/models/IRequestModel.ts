import type {IModel} from "@/modules/common/models/IModel";

export default interface IRequestModel<T extends IModel, K> {
    toRequest(): K;
}
